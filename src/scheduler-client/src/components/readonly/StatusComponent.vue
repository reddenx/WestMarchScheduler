<template>
  <div class="container">
    <div class="row">
      <div class="col" v-for="status in statuses" :key="status">
        <div :class="{'active': session.status == status}">{{ status }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import { SessionViewmodel } from "../../scripts/CommonModels";
export default {
  props: {
    session: SessionViewmodel,
  },
  data: () => ({
    statuses: ["planning", "open", "closed"],
  }),
  mounted() {},
};
</script>

<style>
.active {
  background-color: red;
}
</style>