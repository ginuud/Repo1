import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Player } from "~/types/player";

export const useTeamStore = defineStore('tiim', () => {
    const players = ref<Player[]>([
        {id: 1, name: 'Ivo Linna', team: 'A', points: 0 },
        {id: 2, name: 'Arvo Pärt', team: 'A', points: 15 },
        {id: 3, name: 'Kristjan Jõekalda', team: 'A', points: 0 },
        {id: 4, name: 'Ott Sepp', team: 'A', points: 0 },
        {id: 5, name: 'Jüri Ratas', team: 'A', points: 6 },
        {id: 6, name: 'Koit Toome', team: 'B', points: 0 },
        {id: 7, name: 'Anu Saagim', team: 'B', points: 23 },
        {id: 8, name: 'Evelin Ilves', team: 'B', points: 0 },
        {id: 9, name: 'Marko Reikop', team: 'B', points: 20 },
        {id: 10, name: 'Mihkel Raud', team: 'B', points: 19 },
      ]);

      return { players };
  })