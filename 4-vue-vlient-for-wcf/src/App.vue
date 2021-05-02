<template>
  <div id="app">
      <h1>VUE client for WCF backend</h1>
      <AllTable/>
  </div>
</template>

<script>

import AllTable from './components/AllTable.vue'


export default {
  name: 'App',
  components: {
    AllTable
    
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
