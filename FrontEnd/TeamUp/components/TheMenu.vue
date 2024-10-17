<template>
    <UHorizontalNavigation
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
    {
      label: "Add players",
      to: "/add-player",
    },
    {
      label: "Add teams",
      to: "/add-team",
    },
    {
      label: "Teams",
      to: "/teams",
    },
    {
      label: "Start Game",
      to: "/start-game",
    },
    {
      label: "Games",
      to: "/games",
    },
    {
      label: "Team generation",
      to: "/team-generator",
    },
  ];
  </script>