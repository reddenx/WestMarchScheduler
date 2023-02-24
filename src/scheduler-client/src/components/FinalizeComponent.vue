<template>
  <div>
    <ScheduleComponent
      :startDate="earliestDate"
      :endDate="latestDate"
      :startHour="8"
      :endHour="22"
      :selectableHours="selectableHours"
      v-model="selectedHours"
      :selectedHours="selectedHours"
    />
    <button type="button"
      class="btn btn-primary mt-2"
      @click="submitPressed"
      :disabled="submitting">
      Submit final schedule and Close
    </button>
  </div>
</template>

<script>
import ScheduleComponent, {
  ScheduleDaySelections,
} from "../components/ScheduleComponent2.vue";
import { SessionViewmodel } from "../scripts/CommonModels";
import Api, { ScheduleDatesInputDto } from "../scripts/SessionApi";
import {
  TimeSpan,
  HourBlock,
  timeSpansToHourBlocks,
  hourBlocksToTimeSpans,
} from "../scripts/TimeHelper";

/**
 * input:
 * - player schedules
 */
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
    /** @type {HourBlock[]} */
    selectableHours: [],
    /** @type {HourBlock[]} */
    selectedHours: [],
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

    let playerSpans = [];
    this.session.players.forEach((p) =>
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
      let success = await api.hostFinalize(
        this.session.hostKey,
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