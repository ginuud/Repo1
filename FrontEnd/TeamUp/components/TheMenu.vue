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
  ];
  </script>