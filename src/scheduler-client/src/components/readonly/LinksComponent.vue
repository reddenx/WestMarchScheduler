<template>
    <div class="container">
        <div class="row">
            <div v-if="session.leadKey" class="col-12 col-lg lead-link">
                <router-link class="scheduler-link btn btn-lg btn-danger" :to="`/${session.leadKey}`">
                    <span class="fa-regular fa-clone"></span> 
                    Your Link
                </router-link>
            </div>
            <div v-if="session.hostKey" class="col-12 col-lg host-link">
                <router-link class="scheduler-link btn btn-lg btn-warning" :to="`/${session.hostKey}`">
                    <span class="fa-regular fa-clone"></span>
                    <span v-if="session.lead"> Host Link For "{{ session.lead.name }}"</span>
                    <span v-if="!session.lead"> Host Link</span>
                </router-link>
            </div>
            <div class="col-12 col-lg">
                <router-link class="scheduler-link btn btn-lg btn-success" :to="`/${session.playerKey}`">
                    <span class="fa-regular fa-clone"></span>
                    Link For Players
                </router-link>
            </div>
        </div>
    </div>
</template>

<script>
import { SessionViewmodel } from "../../scripts/CommonModels";
export default {
    props: {
        session: SessionViewmodel,
    },
};
</script>

<style>
.scheduler-link {
    width: 100%;
}
</style>