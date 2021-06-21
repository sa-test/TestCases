from __future__ import print_function
import os.path
from googleapiclient.discovery import build
from google_auth_oauthlib.flow import InstalledAppFlow
from google.auth.transport.requests import Request
from google.oauth2.credentials import Credentials
import pandas as pd
import numpy as np
import datetime
#import pprint as pp

#
# Firstly, put your google credentials to credentials.json
# First time script will request access to spreadsheets and drive (using web site) and save token into token.json
# Next time, script will run using saved token without additional requests
#

# If modifying these scopes, delete the file token.json.
SCOPES = ['https://www.googleapis.com/auth/spreadsheets', 'https://www.googleapis.com/auth/drive']

# The ID and range of a sample spreadsheet.
#actual test sheet https://docs.google.com/spreadsheets/d/1Ycg7zTxds9DZnDvTrFcyNNKuTUxg6Yy6WF0a8Wc02WQ
SPREADSHEET_ID = '1Ycg7zTxds9DZnDvTrFcyNNKuTUxg6Yy6WF0a8Wc02WQ'

def gsheet_api_check(SCOPES):
    creds = None
    if os.path.exists('token.json'):
        #print('Getting creds from file...')
        creds = Credentials.from_authorized_user_file('token.json', SCOPES)
    # If there are no (valid) credentials available, let the user log in.
    if not creds or not creds.valid:
        if creds and creds.expired and creds.refresh_token:
            #print('Refreshing creds...')
            creds.refresh(Request())
        else:
            #print('Getting creds from web...')
            flow = InstalledAppFlow.from_client_secrets_file(
                'credentials.json', SCOPES)
            creds = flow.run_local_server(port=0)
        # Save the credentials for the next run
        with open('token.json', 'w') as token:
            token.write(creds.to_json())
    #print('Creds is done!')
    return creds

def pull_sheet_data(SCOPES,SPREADSHEET_ID,DATA_TO_PULL):
    #print('Checking API...')
    creds = gsheet_api_check(SCOPES)
    #print('Creating service...')
    service = build('sheets', 'v4', credentials=creds)
    sheet = service.spreadsheets()
    result = sheet.values().get(
        spreadsheetId=SPREADSHEET_ID,
        range=DATA_TO_PULL).execute()
    values = result.get('values', [])
    
    if not values:
        print('No data found.')
    else:
        rows = sheet.values().get(spreadsheetId=SPREADSHEET_ID,
                                  range=DATA_TO_PULL).execute()
        data = rows.get('values')
        #print("COMPLETE: Data copied")
        return data

def create_new_sheet(SCOPES, document_title, page_title):
    #Create 2 worksheets:
    # sheetId=0 name=page_title for data source
    # sheetId=1 name='Final report' for pivot table
    creds = gsheet_api_check(SCOPES)
    service = build('sheets', 'v4', credentials=creds)
    spreadsheet = service.spreadsheets().create(body = {
    'properties': {'title': document_title, 'locale': 'ru_RU'},
    'sheets': [{'properties': {'sheetType': 'GRID',
                               'sheetId': 0,
                               'title': page_title,
                               'gridProperties': {'rowCount': 3500, 'columnCount': 25}}},
                {'properties': {'sheetType': 'GRID',
                               'sheetId': 1,
                               'title': 'Final report',
                               'gridProperties': {'rowCount': 500, 'columnCount': 25}}}]
    }).execute()
    spreadsheetId = spreadsheet['spreadsheetId']
    print('https://docs.google.com/spreadsheets/d/' + spreadsheetId)
    return spreadsheetId

def save_df_to_sheet(SCOPES, spreadsheetid, range_name, df):
    creds = gsheet_api_check(SCOPES)
    service = build('sheets', 'v4', credentials=creds)
    result = service.spreadsheets().values().update(
        spreadsheetId=spreadsheetid,
        valueInputOption='RAW',
        range=range_name,
        body=dict(
            majorDimension='ROWS',
            values=df.T.reset_index().T.values.tolist())
    ).execute()
    return result

def create_pivot(SCOPES, spreadsheet_id):
    creds = gsheet_api_check(SCOPES)
    service = build('sheets', 'v4', credentials=creds)
    result = service.spreadsheets().batchUpdate(
        spreadsheetId=spreadsheet_id,
        body=request_body
    ).execute()
    return result


#function to check new_lead
def get_min_trans_date(row, df):
    temp = df.loc[df['l_client_id'] == row['l_client_id']]
    return temp['created_at'].min()

#function to check buy_in_week
def is_buyer(row, df):
    temp = df.loc[df['l_client_id'] == row['l_client_id']]
    buy_date = row['created_at'] + datetime.timedelta(days=7)
    
    temp2 = temp[(temp['created_at'] >= row['created_at']) & (temp['created_at'] <= buy_date)]
    
    if len(temp2) > 0:
        return 1
    else:
        return 0

#calculate revenue from new_buyers
def rev_from_nb(row, df):
    if row['new_buyer']==0:
        return 0
    temp = df.loc[(df['l_client_id'] == row['l_client_id'])]
    return temp['m_real_amount'].sum()




print('Getting managers table...')
data = pull_sheet_data(SCOPES,SPREADSHEET_ID,'managers')
df_managers = pd.DataFrame(data[1:], columns=data[0])

print('Getting leads table...')
data = pull_sheet_data(SCOPES,SPREADSHEET_ID,'leads')
df_leads = pd.DataFrame(data[1:], columns=data[0])

print('Getting transactions table...')
data = pull_sheet_data(SCOPES,SPREADSHEET_ID,'transactions')
df_trans = pd.DataFrame(data[1:], columns=data[0])



print('Checking datatypes...')
if df_leads['created_at'].dtype == 'object':
    print('Leads: created_at is a Object! Converting it to Datetime...')
    df_leads['created_at'] = pd.to_datetime(df_leads['created_at'], errors='coerce')
else:
    print('Leads: created_at is a ',  df_leads['created_at'].dtype)

if df_trans['created_at'].dtype == 'object':
    print('Transactions: created_at is a Object! Converting it to Datetime...')
    df_trans['created_at'] = pd.to_datetime(df_trans['created_at'], errors='coerce')
else:
    print('Transactions: created_at is a ',  df_trans['created_at'].dtype)

print('Transactions: m_real_amount is a Object! Converting it to float...')
df_trans['m_real_amount'] = pd.to_numeric(df_trans['m_real_amount'], downcast='float', errors='coerce')


print('Merging tables leads and managers...')
df_merge1 = pd.merge(df_leads, df_managers, left_on="l_manager_id", right_on="manager_id", how="left")

#Add Trash Lead column
print('Add \'Trash Lead\' column...')
df_merge1['trash_lead'] = np.where(df_merge1['l_client_id']== '00000000-0000-0000-0000-000000000000', 1, 0)


#Add Rank to every 'created_at' for 'l_client_id' 
print('Add rank for clients in leads...')
df_merge1['rank'] = df_merge1.groupby(['l_client_id'])['created_at'].rank(ascending=True)

print('Add rank for transactions...')
df_trans['rank'] = df_trans.groupby(['l_client_id'])['created_at'].rank(ascending=True)

print('Add \'first_trans_date\' column...')
df_merge1['first_trans_date'] = df_merge1.apply(func=get_min_trans_date, df=df_trans, axis=1)

print('Add \'new_lead\' column...')
df_merge1['new_lead'] = np.where((df_merge1['rank']==1 & 
    (pd.isnull(df_merge1['first_trans_date']) | (df_merge1['created_at'] <= df_merge1['first_trans_date']))), 1, 0)

print('Add \'buy_in_week\' column...')
df_merge1['buy_in_week'] = df_merge1.apply(func=is_buyer, df=df_trans, axis=1)

print('Add \'new_buyer\' column...')
df_merge1['new_buyer'] = np.where(((df_merge1['new_lead'] == 1) & (df_merge1['buy_in_week'] == 1)), 1, 0)

print('Add \'rev_from_newbuy\' column...')
df_merge1['rev_from_newbuy'] = df_merge1.apply(func=rev_from_nb, df=df_trans, axis=1)

#print('Saving xls files (for debug)...')
#df_merge1.to_excel("df_merge1.xlsx")
#df_trans.to_excel("df_trans.xlsx")

print('Checking if exist result spreadsheet...')
if os.path.exists('result_spreadsheet_id.txt'):
    with open('result_spreadsheet_id.txt', 'r') as file:
        new_spreadsheet_id = file.read().replace('\n', '')
    print('Resulting spreadsheet \'xo-test-res\': https://docs.google.com/spreadsheets/d/' + new_spreadsheet_id)
else:
    print('Creating new spreadsheet filename=\'xo-test-res\'...')
    new_spreadsheet_id = create_new_sheet(SCOPES, 'xo-test-res', 'source')
    with open('result_spreadsheet_id.txt', 'w') as f:
        f.write(new_spreadsheet_id)
    print('Result spreadsheet_id saved in file result_spreadsheet_id.txt')
    print('Resulting spreadsheet \'xo-test-res\': https://docs.google.com/spreadsheets/d/' + new_spreadsheet_id)

print('Converting datetime to string...')
df_merge1['created_at'] = df_merge1['created_at'].dt.strftime('%Y-%m-%d %H:%M:%S')
df_merge1['first_trans_date'] = df_merge1['first_trans_date'].dt.strftime('%Y-%m-%d %H:%M:%S')

print('Updating result spreadsheet...')

df_merge1.fillna('', inplace=True)
res = save_df_to_sheet(SCOPES, new_spreadsheet_id, 'source!A1', df_merge1)
print(res.keys())
print(res.values())
print(res.get('updatedRange'))

# PivotTable JSON Template
request_body = {
    'requests': [
        {
            'updateCells': {
                'rows': {
                    'values': [
                        {
                            'pivotTable': {
                                # Data Source
                                'source': {
                                    'sheetId': '0',
                                    'startRowIndex': 0,
                                    'startColumnIndex': 0,
                                    'endRowIndex': 3338, # len(df_merge1)+1
                                    'endColumnIndex': 16 # base index is 1
                                },
                                
                                # Rows Field(s)
                                'rows': [
                                    # row field #1
                                    {
                                        'sourceColumnOffset': 3, # d_utm_source
                                        'showTotals': True, # display subtotals
                                        'sortOrder': 'ASCENDING',
                                        'repeatHeadings': False,
                                        'label': 'Канал привлечения заявки (d_utm_source)',
                                    },

                                    {
                                        'sourceColumnOffset': 8, # d_club
                                        'showTotals': True, # display subtotals
                                        'sortOrder': 'ASCENDING',
                                        'repeatHeadings': False,
                                        'label': 'Клуб (d_club)',
                                    },
                                    
                                    {
                                        'sourceColumnOffset': 7, # d_manager
                                        'showTotals': False, # display subtotals
                                        'sortOrder': 'ASCENDING',
                                        'repeatHeadings': True,
                                        'label': 'Менеджер (d_manager)',
                                    }                               
                                ],

                                # Columns Field(s)
                                #'columns': [
                                #    # column field #1
                                #    {
                                #        'sourceColumnOffset': 14,
                                #        'sortOrder': 'ASCENDING', 
                                #        'showTotals': True
                                #    }
                                #],

                                # Values Field(s)
                                'values': [
                                    # value field #1
                                    {
                                        'sourceColumnOffset': 0, #lead_id
                                        'summarizeFunction': 'COUNTA',
                                        'name': 'кол. заявок:'
                                    },
                                    
                                    {
                                        'sourceColumnOffset': 9, #trash_lead
                                        'summarizeFunction': 'SUM',
                                        'name': 'кол. мусорных заявок:'
                                    },
                                    
                                    {
                                        'sourceColumnOffset': 12, #new_lead
                                        'summarizeFunction': 'SUM',
                                        'name': 'кол. новых заявок:'
                                    },
                                    
                                    {
                                        'sourceColumnOffset': 13, #buy_in_week
                                        'summarizeFunction': 'SUM',
                                        'name': 'кол. покупателей:'
                                    },
                                    
                                    {
                                        'sourceColumnOffset': 14, #new_buyer 
                                        'summarizeFunction': 'SUM',
                                        'name': 'кол. новых покупателей:'
                                    },
                                    
                                    {
                                        'sourceColumnOffset': 15, #rev_from_newbuy
                                        'summarizeFunction': 'SUM',
                                        'name': 'доход от покупок новых покупателей:'
                                    }                                    
                                ],

                                'valueLayout': 'HORIZONTAL'
                            }
                        }
                    ]
                },
                
                'start': {
                    'sheetId': '1',
                    'rowIndex': 0, # 1th row
                    'columnIndex': 0 # 1rd column
                },
                'fields': 'pivotTable'
            }
        }
    ]
}

print('Creating final report (pivot)...')
res = create_pivot(SCOPES, new_spreadsheet_id)
print(res)

print('All done!')




