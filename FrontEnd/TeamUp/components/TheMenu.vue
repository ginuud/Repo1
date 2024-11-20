<!-- <template>
  <UHorizontalNavigation
    v-if="route.name !== 'index'"
    :links="links"
    class="border-b border-gray-200 dark:border-gray-800 flex justify-center text-purple-500"
    >
  </UHorizontalNavigation>
  <div>
    <Logo />
  </div>
</template> -->

<script setup lang="ts">
import {
  Menubar,
  MenubarMenu,
  MenubarTrigger,
} from '@/components/ui/menubar'

const items = ref([
  {
    label: "Players",
    path: "/players",
  },
  {
    label: "Add players",
    path: "/add-player",
  },
  {
    label: "Add teams",
    path: "/add-team",
  },
  {
    label: "Teams",
    path: "/teams",
  },
  {
    label: "Start Game",
    path: "/start-game",
  },
  {
    label: "Games",
    path: "/games",
  },
  {
    label: "Team generation",
    path: "/team-generator",
  },
]);
</script>

<template>
  <Menubar class="justify-center">
    <MenubarMenu v-for="(item, index) in items" :key="index" class= "justify-center">
      <a :href="item.path">
        <MenubarTrigger>{{ item.label }}</MenubarTrigger>
      </a>
    </MenubarMenu>
  </Menubar>
</template>
