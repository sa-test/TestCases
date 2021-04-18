<template>
    <div>
      <h3>Table from WCF backend</h3>
      
      <div class="mytable">
        <!--
          <div v-for="myrow in GetMyTable" v-bind:key="myrow.id" class="myrow">
              {{ myrow.Name }}
          </div>
        -->
        <table border="1" align="center">
            <tr><th>ID</th> <th>Name</th> <th>Description</th> <th>Edit</th> <th>Delete</th></tr>
            <tr v-for="myrow in GetMyTable" v-bind:key="myrow.id" class="myrow">
                <td>{{myrow.id}}</td>
                <td>{{myrow.Name}}</td>
                <td>{{myrow.Description}}</td>
                <td><button v-on:click="doUpdate(myrow.id)"> Edit</button></td>
                <td><button v-on:click="deleteRow(myrow.id)"> Delete</button></td>
            </tr>
            <tr>
                <td><button v-on:click="addRow({newName, newDescr})">Add</button></td>
              
                <td><input v-model="newName" placeholder="New Name"></td>
                <td><input v-model="newDescr" placeholder="Description"></td>
                
            </tr>
        </table>

        <div v-if="GetUpdateStatus > 0">
          <h3>Update row:</h3>
            <table align="center">
                <tr>
                    <th> </th><th>ID</th>  <th> Name </th>  <th> Description </th>
                </tr>
                <tr>
                    <td>Old value: </td>
                    <td>{{GetUpdateStatus}}</td>
                    <td>{{GetMyTable.find(i=>i.id===GetUpdateStatus).Name}}</td>
                    <td>{{GetMyTable.find(i=>i.id===GetUpdateStatus).Description}}</td>
                    
                </tr>
                <tr>
                    <td>New value: </td>
                    <td>{{GetUpdateStatus}}</td>
                    <td><input type="text" v-model="updateName"></td>
                    <td><input type="text" v-model="updateDescr"></td>
                    
                </tr>

            </table>
            <button @click="doUpdate(0)"> Cancel Update</button>
            <button @click="updateRow({updateName, updateDescr})"> Update </button>
        </div>

      </div>
      
  </div>
</template>

<script>
import {mapGetters, mapActions} from 'vuex';

export default {
    name: "AllTable",

    data: function() {
        return {
          newName: this.newName,
          newDescr: this.newDescr,
          updateName: this.updateName,
          updateDescr: this.updateDescr
          
         }
    },
     methods: {
        ...mapActions(['fetchTable', 'addRow', 'deleteRow', 'doUpdate', 'updateRow', 'GetName'])
    },
    computed: mapGetters(['GetMyTable', 'GetUpdateStatus']),
    created() {
        console.log("AllTable-Created...");
        this.fetchTable();
    }
 
}

</script>

<style>

</style>