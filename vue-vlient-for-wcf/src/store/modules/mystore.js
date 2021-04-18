import axios from 'axios'

const state = {
    mystore: [],
    update: 0
};

const getters = {
    //GetMyTable: (state) => state.mytable
    GetMyTable: function(state) {
        console.log("Step into GetMyTable...")
        return state.mystore
    },
    GetUpdateStatus: function(state) {
        return state.update
    },
    GetName: function(state) {
        return state.mystore.find(i=>i.id===state.update).Name
    }
};

const actions = {
    async fetchTable ({commit}) {
        console.log("Step into fetchTable...")
        const response = await axios.get('http://localhost:5657/TestDBService.svc/web/GetData')
        console.log(response)
        console.log(response.data)
        let listOfObjects = Object.keys(response.data).map((key) => {
            return response.data[key]
          })
        console.log(listOfObjects)
        console.log(listOfObjects[0])
        //const response2 = await axios.get('https://jsonplaceholder.typicode.com/todos')
        //console.log(response2)
        //console.log(response2.data)

        commit('setTable', listOfObjects[0])
    },
    async addRow({commit}, data) {
        console.log("Step into addRow action...")
        console.log(data.newName, data.newDescr);
        const response1 = await axios.post(`http://localhost:5657/TestDBService.svc/web//AddUser/${data.newName}/${data.newDescr}`)
        console.log(response1.data)
        const response = await axios.get('http://localhost:5657/TestDBService.svc/web/GetData')
        let listOfObjects = Object.keys(response.data).map((key) => {
            return response.data[key]
          })
        console.log(listOfObjects)
        console.log(listOfObjects[0])
        commit('setTable', listOfObjects[0])        
        
    },
    async deleteRow({commit}, id) {
        await axios.get(`http://localhost:5657/TestDBService.svc/web/DelUser/${id}`);
        const response = await axios.get('http://localhost:5657/TestDBService.svc/web/GetData')
        let listOfObjects = Object.keys(response.data).map((key) => {
            return response.data[key]
          })
        console.log(listOfObjects)
        console.log(listOfObjects[0])
        commit('setTable', listOfObjects[0])  

    },
    doUpdate({commit}, status) {
        console.log("Step into doUpdate")
        console.log(status)
        commit('setUpdate', status)
    },
    async updateRow({commit}, params) {
        console.log(params)
        let id = this.getters.GetUpdateStatus
        console.log(id)
        const response1 = await axios.get(`http://localhost:5657/TestDBService.svc/web/UpdateUserWeb/${id}/${params.updateName}/${params.updateDescr}`);
        console.log(response1.data)
        const response = await axios.get('http://localhost:5657/TestDBService.svc/web/GetData')
        let listOfObjects = Object.keys(response.data).map((key) => {
            return response.data[key]
          })

        commit('setTable', listOfObjects[0])
    }

};

const mutations = {
    setTable: (state, table) => (state.mystore = table),
    //newTodo: (state, todo) => (state.todos.unshift(todo)),
    //removeRow: (state, id) => state.table = state.table.filter(row => row.id != id),
    setUpdate: function(state, status) {
        state.update = status
        console.log(state.update)
    }
};

export default {
    state,
    getters,
    actions,
    mutations
}