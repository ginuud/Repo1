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
import { Menubar, MenubarMenu, MenubarTrigger } from "@/components/ui/menubar";

const route = useRoute();

const links = ref([
  {
    label: "Players",
    to: "/players",
  },
  {
    label: "Teams",
    to: "/teams",
  },
  {
    label: "Games",
    to: "/games",
  },
]);
</script>

<!-- <template>
  <div>
  <Menubar class="justify-center gap-8 flex hover:scale-[101%] transition cursor-pointer hero" >
    <MenubarMenu v-for="(item, index) in items" :key="index" >
      <a :href="item.path" className="text-slate-400">
        <MenubarTrigger class="flex items-center gap-2 px-2 py-1 transition 
        rounded cursor-pointer hover:bg-neutral-100 text-lg">{{ item.label }}</MenubarTrigger>
      </a>
    </MenubarMenu>
  </Menubar>
</div>
</template> -->

<template>
  <UHorizontalNavigation
    v-if="route.name !== 'index'"
    :links="links"
    class="justify-center hero"
    :ui="{
      after: '',
      active: 'dark:text-[#202a79]',
      base: 'rounded-md flex font-medium text-xl px-8 py-8',
    }"
  />
</template>

<style scoped>
.hero {
  background-color: #e8e8d9;
}
</style>
