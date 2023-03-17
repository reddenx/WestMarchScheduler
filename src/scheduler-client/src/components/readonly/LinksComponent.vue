<template>
    <div class="container">
        <div class="row">
            <div class="col-12 col-lg-4">
                <span class="">
                    Host:
                    <span class="badge bg-warning"
                        ><span class="fa-regular fa-clone"></span>
                        {{ session.host.name }}
                    </span>
                </span>
            </div>
            <div class="col-12 col-lg-4">Lead: {{ session.lead.name }}</div>
            <div class="col-12 col-lg-4">Player Link</div>
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
</style>