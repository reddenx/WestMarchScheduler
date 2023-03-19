<template>
  <div class="container">
    <div class="row">
      <div class="col-12">
        <!-- DONE -->
        <StatusComponent :session="session" />
      </div>
      <div class="col-12">
        <!-- done enough for now -->
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
        "pkey",
        "open",
        "let's go get that druid compound",
        "A long description here, sometimes people tend to write a very long description. maybe let's go to the next town over and murder a bunch of people assuming they're undead before realizing there was just an outbreak of cholera. and now have to disguise yourselves on the main road anywhere near that region",
        {
          name: "Sean",
          schedule: [
            {
              start: new Date(2023, 2, 24, 13).toISOString(),
              end: new Date(2023, 2, 24, 17).toISOString(),
            },
          ],
        },
        {
          name: "Drauno",
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
          {
            name: "Himby",
            schedule: [
              {
                start: new Date(2023, 2, 24, 13).toISOString(),
                end: new Date(2023, 2, 24, 17).toISOString(),
              },
            ],
          },
          {
            name: "Halls",
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