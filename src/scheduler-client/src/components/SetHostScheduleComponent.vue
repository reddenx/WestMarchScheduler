<template>
  <div>
    Please enter your name and schedule
    <input type="text" v-model="name" class="form-control mb-2" placeholder="NAME" />
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
import Api, { ScheduleDatesInputDto } from "../scripts/SessionApi";
import {
  TimeSpan,
  HourBlock,
  timeSpansToHourBlocks,
  hourBlocksToTimeSpans,
} from "../scripts/TimeHelper";

export default {
  components: {
    ScheduleComponent,
  },
  props: {
    hostKey: String,
  },
  data: () => ({
    startDate: new Date(),
    endDate: new Date(),
    selectableHours: [],
    /** @type {ScheduleDaySelections[]} */
    selectedHours: [],
    name: '',
    submitting: false
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
    async submitPressed() {
      this.submitting = true;
      let api = new Api();

      let hours = [];
      this.selectedHours.forEach((day) => {
        day.hours.forEach((hour) => {
          hours.push(
            new Date(day.date.getFullYear(), day.date.getMonth(), day.date.getDate(), hour)
          );
        });
      });

      let spans = hourBlocksToTimeSpans(hours.map((h) => new HourBlock(h)));

      console.log(spans);

      let success = await api.approveSession(
        this.hostKey,
        this.name,
        spans.map((h) => new ScheduleDatesInputDto(h.start, h.end))
      );
      this.submitting = false;
      if(success) {
        this.$emit('submitted');
      }
    },
  },
};
</script>

<style>
</style>