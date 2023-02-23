<template>
  <div>
    Welcome back {{ session.lead.name }}, please enter your schedule
    <ScheduleComponent 
      :startDate="earliestDate"
      :endDate="latestDate"
      :startHour="6"
      :endHour="23"
      :selectableHours="selectableHours"
      v-model="selectedHours"
    />

    <br />
    {{ earliestDate }} <br />
    {{ latestDate }} <br />


    <button :disabled="submitting" type="button" class="btn btn-primary mt-2" @click="submitPressed">
      Submit</button>


  </div>
</template>

<script>
import { SessionViewmodel, ScheduleViewmodel } from "../scripts/CommonModels";
import Api, { ScheduleDatesInputDto } from "../scripts/SessionApi";
import ScheduleComponent from "./ScheduleComponent2.vue"
import {
  TimeSpan,
  HourBlock,
  timeSpansToHourBlocks,
  hourBlocksToTimeSpans,
} from "../scripts/TimeHelper";

export default {
  components: { ScheduleComponent },
  props: {
    session: SessionViewmodel
  },
  data: () => ({
    earliestDate: new Date(),
    latestDate: new Date(),
    selectableHours: [],
    selectedHours: {},
    submitting: false,
  }),
  mounted() {
    this.earliestDate = new Date(Math.min(... this.session.host.schedule.dates.map(d => d.start)));
    this.latestDate = new Date(Math.max(... this.session.host.schedule.dates.map(d => d.end)));

    let spans = this.session.host.schedule.dates.map(d => new TimeSpan(d.start, d.end));
    let blocks = timeSpansToHourBlocks(spans);
    this.selectableHours.push(...blocks.map(b => b.hour));
  },
  methods: {
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

      let success = await api.leadSchedule(
        this.session.leadKey,
        spans.map((h) => new ScheduleDatesInputDto(h.start, h.end))
      );
      this.submitting = false;
      if(success) {
        this.$emit('submitted');
      }

    },
  },

}
</script>

<style>

</style>
