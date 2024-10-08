<template>
    <div>
      <h1 class="text-xl text-center">{{ title }}</h1>
      <template v-if="players.length > 0">
        <UTable v-model:sort="sort" :direction="asc" :columns="columns" :rows="players">
        </UTable>
      </template>
      <template v-else>
        <p class="text-center">Mängijate andmed puuduvad</p>
      </template>
    </div>
  </template>

<script setup lang ="ts">
    defineProps<{ title: String }>();
    const columns = [
    {
      key: "name",
      label: "Name",
    },
    {
      key: "team",
      label: "Team",
    },
    {
      key: "points",
      label: "Points",
      sortable: true
    },
    {
      key: "actions",
      label: "Tegevused"
    }
  ];

    const {players} = usePlayerStore();

</script>