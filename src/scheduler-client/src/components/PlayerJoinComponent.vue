<template>
  <div>
    Please enter your name and schedule
    <input
      type="text"
      v-model="name"
      class="form-control mb-2"
      placeholder="NAME"
    />
    <ScheduleComponent
      :startDate="earliestDate"
      :endDate="latestDate"
      :startHour="8"
      :endHour="22"
      :selectableHours="selectableHours"
      v-model="selectedHours"
      :selectedHours="selectedHours"
    />
    <button
      class="btn btn-primary mt-2"
      type="button"
      @click="submitPressed"
      :disabled="submitting"
    >
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
    name: "",
    submitting: false,
  }),
  mounted() {
    this.earliestDate = new Date(
      Math.min(...this.session.lead.schedule.dates.map((d) => d.start))
    );
    this.latestDate = new Date(
      Math.max(...this.session.lead.schedule.dates.map((d) => d.end))
    );

    let spans = this.session.lead.schedule.dates.map(
      (d) => new TimeSpan(d.start, d.end)
    );
    let blocks = timeSpansToHourBlocks(spans);
    this.selectableHours.push(...blocks.map((b) => b.hour));

    /**@type {SessionViewmodel} */
    let session = this.session;
    let playerSpans = [];

    session.players.forEach((p) =>
      playerSpans.push(
        ...p.schedule.dates.map((d) => new TimeSpan(d.start, d.end))
      )
    );
    let playerBlocks = timeSpansToHourBlocks(playerSpans);
    this.selectedHours.push(...playerBlocks.map((b) => b.hour));
  },
  methods: {
    async submitPressed() {
      this.submitting = true;

      let hours = [];
      this.selectedHours.forEach((day) => {
        day.hours.forEach((hour) => {
          hours.push(
            new Date(
              day.date.getFullYear(),
              day.date.getMonth(),
              day.date.getDate(),
              hour
            )
          );
        });
      });

      let spans = hourBlocksToTimeSpans(hours.map((h) => new HourBlock(h)));

      let api = new Api();
      let success = await api.playerJoin(
        this.session.playerKey,
        this.name,
        spans.map((h) => new ScheduleDatesInputDto(h.start, h.end))
      );

      if(success) {
        this.$emit('reload');
      }
      this.submitting = false;
    },
  },
};
</script>

<style>
</style>