<template>
  <div>
    Please enter your name and schedule
    <input type="text" v-model="name" class="form-control mb-2" placeholder="NAME" />
    <ScheduleComponent :startDate="earliestDate" :endDate="latestDate" :startHour="8" :endHour="22"
      :selectableHours="selectableHours" v-model="selectedHours" />
    <button class="btn btn-primary mt-2" type="button" @click="submitPressed">
      Submit
    </button>
  </div>
</template>

<script>
import ScheduleComponent, {
  ScheduleDaySelections,
} from "../components/ScheduleComponent.vue";
import { SessionViewmodel } from "../scripts/CommonModels";
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
    session: SessionViewmodel,
  },
  data: () => ({
    earliestDate: new Date(),
    latestDate: new Date(),
    selectableHours: [],
    /** @type {ScheduleDaySelections[]} */
    selectedHours: [],
    name: '',
    submitting: false
  }),
  mounted() {
    this.earliestDate = new Date(Math.min(... this.session.host.schedule.dates.map(d => d.start)));
    this.latestDate = new Date(Math.max(... this.session.host.schedule.dates.map(d => d.end)));

    let spans = this.session.host.schedule.dates.map(d => new TimeSpan(d.start, d.end));
    let blocks = timeSpansToHourBlocks(spans);
    this.selectableHours.push(...blocks.map(b => b.hour));
  },
  methods: {
    submitPressed() { },
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
  }

}
</script>

<style>

</style>