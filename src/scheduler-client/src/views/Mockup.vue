<template>
  <div class="container">
    <div class="row">
      <div class="col-12">
        <StatusComponent :session="session" />
      </div>
      <div class="col-12">
        <InfoComponent :session="session" />
      </div>
      <div class="col-12">
        <LinksComponent :session="session" />
      </div>
      <div class="col-12 col-lg-6">CTA HERE</div>
      <div class="col-12 col-lg-6">
        <PlayerListComponent :session="session" />
      </div>
    </div>
  </div>
</template>

<script>
import InfoComponent from "../components/readonly/InfoComponent.vue";
import StatusComponent from "../components/readonly/StatusComponent.vue";
import PlayerListComponent from "../components/readonly/PlayerListComponent.vue";
import LinksComponent from "../components/readonly/LinksComponent.vue";
import { SessionViewmodel } from "../scripts/CommonModels";
import { SessionDto } from "../scripts/SessionApi";

export default {
  components: {
    InfoComponent,
    StatusComponent,
    LinksComponent,
    PlayerListComponent,
  },
  data: () => ({
    session: new SessionViewmodel(
      new SessionDto(
        "pkey",
        "hkey",
        "lkey",
        "open",
        "session title here",
        "a long description here",
        {
          name: "Jonk the DM",
          schedule: [
            {
              start: new Date(2023, 2, 24, 13).toISOString(),
              end: new Date(2023, 2, 24, 17).toISOString(),
            },
          ],
        },
        {
          name: "Drauno the Eager",
          schedule: [
            {
              start: new Date(2023, 2, 24, 13).toISOString(),
              end: new Date(2023, 2, 24, 17).toISOString(),
            },
          ],
        },
        [
          {
            name: "Jonk",
            schedule: [
              {
                start: new Date(2023, 2, 24, 13).toISOString(),
                end: new Date(2023, 2, 24, 17).toISOString(),
              },
            ],
          },
          {
            name: "Jimbo",
            schedule: [
              {
                start: new Date(2023, 2, 24, 13).toISOString(),
                end: new Date(2023, 2, 24, 17).toISOString(),
              },
            ],
          },
        ],
        null,
        "hour"
      ),
      "keeeyyy"
    ),
  }),
  mounted() {},
  methods: {},
};
</script>

<style>
</style>