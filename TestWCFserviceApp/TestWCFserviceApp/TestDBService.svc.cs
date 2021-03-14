using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.Entity;


namespace TestWCFserviceApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class TestDBService : ITestDBService
    {
        public int AddUser(string name, string descr)
        {
            using (TestDB1Entities db = new TestDB1Entities())
            {
                try
                {
                    db.EFUsers.Load();
                    EFUser newuser = new EFUser();
                    newuser.Name = name;
                    newuser.Description = descr;
                    db.EFUsers.Add(newuser);
                    db.SaveChanges();
                    return 1;

                }
                catch (Exception ex)
                {
                    return 0;
                }

            }
        }

        public int DeleteUser(int id)
        {
            using (TestDB1Entities db = new TestDB1Entities())
            {
                try
                {
                    db.EFUsers.Load();
                    EFUser userDel = db.EFUsers.Find(id);
                    if (userDel != null)
                    {
                        db.EFUsers.Remove(userDel);
                        db.SaveChanges();
                        return 1;

                    }
                    else { return 0; }

                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        public int UpdateUser(int id, string name, string descr)
        {
            using (TestDB1Entities db = new TestDB1Entities())
            {
                try
                {
                    db.EFUsers.Load();
                    EFUser userUpd = db.EFUsers.Find(id);
                    if (userUpd != null)
                    {
                        if (String.IsNullOrEmpty(name))
                        {
                            name = "";
                        }

                        if (String.IsNullOrEmpty(descr))
                        {
                            descr = "";
                        }

                        userUpd.Name = name;
                        userUpd.Description = descr;
                        db.SaveChanges();
                        return 1;

                    }
                    else { return 0; }

                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        public List<User> GetData()
        {
            using (TestDB1Entities db = new TestDB1Entities())
            {
                try
                {
                    db.EFUsers.Load();
                    List<User> usersDTO = new List<User>();

                    foreach(EFUser efuser in db.EFUsers)
                    {
                        User user = new User();
                        user.id = efuser.id;
                        user.Name = efuser.Name;
                        user.Description = efuser.Description;
                        usersDTO.Add(user);
                    }

                    return usersDTO;

                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            
        }

        /*
        private ExportContext TranslateEntityToExportClass(TestDB1Entities db)
        {
            ExportContext exportDb = new ExportContext();
            exportDb = db;
            return exportDb;
        }
        */
    }
}
