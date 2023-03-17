<template>
  <div class="container">
    <h1>{{session.title}}</h1>
    <p>
    {{session.description}}
    </p>
  </div>
</template>

<script>
import { SessionViewmodel } from "../../scripts/CommonModels";
export default {
  props: {
    session: SessionViewmodel,
  }
}
</script>

<style>
h1 {
  font-size: 2em;
  text-transform: capitalize;
}
</style>