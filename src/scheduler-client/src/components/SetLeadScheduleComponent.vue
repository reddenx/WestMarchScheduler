<template>
  <div>
    Please enter your name and choice of available schedule.
    <input type="text" v-model="name" class="form-control" />
    <ScheduleComponent />
    <button type="button" class="btn btn-primary mt-2" @click="submitPressed">
      Submit</button>


  </div>
</template>

<script>
import ScheduleComponent from "./ScheduleComponent"

export default {
  components: [ScheduleComponent],
  data: () => ({
  }),
  methods: {
    submitPressed() {
      alert('test');
    },
  },

}
</script>

<style>

</style>
