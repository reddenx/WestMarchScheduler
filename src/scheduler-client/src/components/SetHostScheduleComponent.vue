<template>
  <div>
      Set Host Schedule Component
      <ScheduleComponent 
      :startDate="startDate"
      :endDate="endDate"
      :startHour="8"
      :endHour="22"
      :selectableHours="selectedHours"
      />
  </div>
</template>

<script>
import ScheduleComponent from "../components/ScheduleComponent.vue"


export default {
  components: {
    ScheduleComponent
  },
  data: () => ({
    startDate: new Date(),
    endDate: new Date(2022, 1, 1),
    selectedHours: [
      new Date(2022,0,27,14),
      new Date(2022,0,27,15),
      new Date(2022,0,27,16),
      new Date(2022,0,27,17),
      new Date(2022,0,27,18),
      new Date(2022,0,27,19),
    ]
  }),
  mounted() {
  },
}
</script>

<style>

</style>