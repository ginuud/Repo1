import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Player } from "~/types/player";
import type { Game } from "~/types/game";
import type { Team } from "~/types/team";

export const usePlayerStore = defineStore('player', () => {
    let currentId: number = 0;

    function generateId(): number {
      currentId++;
      return currentId;
    }

    const addPlayer = (player: Player) => {
        players.value.push(player)
    }

    const deletePlayer = async (playerId: number) => {
        const index = players.value.findIndex((player) => player.id === playerId);
        if (index !== -1) {
          players.value.splice(index, 1);
        }
      }

    const players = ref<Player[]>([
        {id: generateId(), name: 'Ivo Linna', points: 0, rank: 10 },
        {id: generateId(), name: 'Arvo Pärt', points: 15, rank: 4 },
        {id: generateId(), name: 'Kristjan Jõekalda', points: 0, rank: 9 },
        {id: generateId(), name: 'Ott Sepp', points: 0, rank: 8 },
        {id: generateId(), name: 'Jüri Ratas', points: 6, rank: 5 },
        {id: generateId(), name: 'Koit Toome', points: 0, rank: 7 },
        {id: generateId(), name: 'Anu Saagim', points: 23, rank: 1 },
        {id: generateId(), name: 'Evelin Ilves', points: 0, rank: 6 },
        {id: generateId(), name: 'Marko Reikop', points: 20, rank: 2 },
        {id: generateId(), name: 'Mihkel Raud', points: 19, rank: 3 },
      ]);

      const generateRanks = () => {
        const sortedPlayers = players.value.slice().sort((a, b) => b.points - a.points);
    
        sortedPlayers.forEach((player, index) => {
          player.rank = index + 1;
        });

        players.value = sortedPlayers;
      };

      return { players, generateId, addPlayer, deletePlayer, generateRanks };
  })

  export const useGameStore = defineStore('game', () => {
    let currentId: number = 0;

    function generateId(): number {
      currentId++;
      return currentId;
    }

    const games = ref<Game[]>([
      {id: generateId(), name: 'Jalgpall', team1name: 'A', team2name: 'B', status: 'in progress'},
      {id: generateId(), name: 'Korvpall', team1name: 'A', team2name: 'B', status: 'inactive'},
    ]);
    const addGame = (game: Game) => {
      games.value.push(game)
  }

  const updateGameStatus = (id: number, status: "in progress" | "inactive") => {
    const game = games.value.find(game => game.id === id);
    if (game) {
      game.status = status;
    }
  }
  return { games, generateId, addGame, updateGameStatus };
  })

  export const useTeamStore = defineStore('team', () => {
    let currentId: number = 0;

    function generateId(): number {
      currentId++;
      return currentId;
    }

    const addTeam = (team: Team) => {
        teams.value.push(team)
    }

    const deleteTeam = async (teamId: number) => {
      const index = teams.value.findIndex((team) => team.id === teamId);
      if (index !== -1) {
        teams.value.splice(index, 1);
      }
    }

    const teams = ref<Team[]>([
      {id: generateId(), teamname: 'A', members:['Ivo Linna', 'Arvo Pärt','Kristjan Jõekalda']},
      {id: generateId(), teamname: 'B', members:['Koit Toome', 'Evelin Ilves']},
    ]);

    return { teams, generateId, addTeam, deleteTeam };
  })