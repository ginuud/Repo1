import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Player } from "~/types/player";

export const useTeamStore = defineStore('tiim', () => {
    const players = ref<Player[]>([
        {id: 1, name: 'Ivo Linna', team: 'A' },
        {id: 2, name: 'Arvo Pärt', team: 'A' },
        {id: 3, name: 'Kristjan Jõekalda', team: 'A' },
        {id: 4, name: 'Ott Sepp', team: 'A' },
        {id: 5, name: 'Jüri Ratas', team: 'A' },
        {id: 6, name: 'Koit Toome', team: 'B' },
        {id: 7, name: 'Anu Saagim', team: 'B' },
        {id: 8, name: 'Evelin Ilves', team: 'B' },
        {id: 9, name: 'Marko Reikop', team: 'B' },
        {id: 10, name: 'Mihkel Raud', team: 'B' },
      ]);

      return { players };
  })