<template>
    <div>
      <h1 class="text-xl text-center">{{ title }}</h1>
      <template v-if="players.length > 0">
        <UTable v-model:sort="sort" :columns="columns" :rows="players"></UTable>
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
      key: "rank",
      label: "Rank",
    },
    {
      key: "points",
      label: "Points",
      sortable: true
    },
    {
      key: "name",
      label: "Name",
    },
    {
      key: "actions",
      label: "Tegevused"
    }
  ];

  const playerStore = usePlayerStore();

  const {players} = storeToRefs(playerStore)

  onMounted(() => {
    playerStore.loadPlayers();
    console.log("Players loaded:", players.value);
  })


    const sort = ref({
    column: 'points',
    direction: 'desc'
  })

</script>