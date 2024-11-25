<template>
  <UHorizontalNavigation
  v-if = "route.name !== 'index'"
    :links="links"
    class="border-b border-gray-200 dark:border-gray-800"
  />
</template>

<script setup lang="ts">
const route = useRoute();

const links = [
  {
    label: "Players",
    to: "/players",
  },
  // {
  //   label: "Add teams",
  //   to: "/add-team",
  // },
  {
    label: "Teams",
    to: "/teams",
  },
  {
    label: "Games",
    to: "/games",
  },
  // {
  //   label: "Team generation",
  //   to: "/team-generator",
  // },
];
</script>
