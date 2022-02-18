<template>
  <div>
    Set Host Schedule Component
    <ScheduleComponent
      :startDate="startDate"
      :endDate="endDate"
      :startHour="8"
      :endHour="22"
      :selectableHours="selectableHours"
      v-model="selectedHours"
    />
    <button class="btn btn-primary mt-2" type="button" @click="submitPressed">
      Submit
    </button>
  </div>
</template>

<script>
import ScheduleComponent, {
  ScheduleDaySelections,
} from "../components/ScheduleComponent.vue";
import Api, { ScheduleDatesDto } from "../scripts/SessionApi";

export default {
  components: {
    ScheduleComponent,
  },
  data: () => ({
    startDate: new Date(2022, 0, 0),
    endDate: new Date(2022, 1, 1),
    selectableHours: [
      new Date(2022, 0, 27, 14),
      new Date(2022, 0, 27, 15),
      new Date(2022, 0, 27, 16),
      new Date(2022, 0, 27, 17),
      new Date(2022, 0, 27, 18),
      new Date(2022, 0, 27, 19),
    ],
    /** @type {ScheduleDaySelections[]} */
    selectedHours: [],
  }),
  mounted() {
    let now = new Date();
    let end = new Date(now.getFullYear(), now.getMonth(), now.getDate() + 30);
    this.recalculateAvailability(new Date(), end);
  },
  methods: {
    recalculateAvailability(startDate, endDate) {
      let allHours = [];
      let start = new Date(startDate);
      while (start < endDate) {
        let tempHours = [];
        for (let i = 8; i <= 21; ++i) {
          tempHours.push(
            new Date(start.getFullYear(), start.getMonth(), start.getDate(), i)
          );
        }
        allHours.push(...tempHours);
        start.setDate(start.getDate() + 1);
      }

      this.startDate = startDate;
      this.endDate = endDate;
      this.selectableHours = allHours;
    },
    submitPressed() {
      let api = new Api();
      new ScheduleDatesDto()
      //TODO translate a bunch of hours into time blocks... ick
      api.approveSession(key, "frank", TODO);
    },
  },
};
</script>

<style>
</style>