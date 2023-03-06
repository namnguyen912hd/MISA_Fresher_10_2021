<template>
  <div>
    <the-navigation></the-navigation>
    <the-header></the-header>
    <the-content></the-content>
  </div>
</template>

<script>
import TheContent from './components/layout/TheContent.vue'
import TheHeader from './components/layout/TheHeader.vue'
import TheNavigation from './components/layout/TheNavigation.vue'

export default {
  name: 'App',
  components: {
    TheNavigation,
    TheHeader,
    TheContent
  }
}
</script>

<style>
  @import url("./assets/css/main.css");
</style>
