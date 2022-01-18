<template>
  <div>Status: {{lookupKey}}
  </div>
</template>

<script>
import Api from "../scripts/SessionApi";
const api = new Api();

export default {
  props: {
    lookupKey: String,
  },
  async mounted() {
      let dto = await api.getSession(this.lookupKey);
      console.log(dto);
  },
};
</script>

<style>
</style>