<template>
  <div class="card">
    <div class="card-body">
      <div class="row">
        <div class="col-12">
            <!-- TODO: add status components by lookuptype, then status -->
          <div class="badge bg-info text-dark float-end">
            {{ session.status }}
          </div>
          <h1>
            {{ session.title }}
          </h1>
          <hr />
          <p class="text-start">
            {{ session.description }}
          </p>
          <hr />
          <div class="text-start">
            <div class="alert alert-danger">
              Lead Link:

              <router-link class="alert-link" :to="`/${session.leadKey}`">{{
                session.leadKey
              }}</router-link>
            </div>
            <div class="alert alert-warning">
              Host Link:
              <router-link class="alert-link" :to="`/${session.hostKey}`">{{
                session.hostKey
              }}</router-link>
            </div>
            <div class="alert alert-success">
              Player Link:
              <router-link class="alert-link" :to="`/${session.playerKey}`">{{
                session.playerKey
              }}</router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { SessionViewmodel } from "../scripts/CommonModels";

export default {
  props: {
    session: SessionViewmodel,
  },
};
</script>

<style>
</style>