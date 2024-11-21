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
    label: "Teams",
    path: "/teams",
  },
  {
    label: "Games",
    path: "/games",
  },
])
</script>

<template>
  <Menubar class="justify-center gap-8 flex hover:scale-[101%] transition cursor-pointer" >
    <MenubarMenu v-for="(item, index) in items" :key="index" >
      <a :href="item.path" className="text-slate-400">
        <MenubarTrigger class="flex items-center gap-2 px-2 py-1 transition 
        rounded cursor-pointer hover:bg-neutral-100 text-lg">{{ item.label }}</MenubarTrigger>
      </a>
    </MenubarMenu>
  </Menubar>
</template>
