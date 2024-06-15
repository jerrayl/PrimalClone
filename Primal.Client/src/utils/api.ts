import axios from 'axios';
import { AttackModel, CreateFreeCompanyModel, CreatePlayerModel, EncounterModel, FreeCompanyModel, JoinFreeCompanyModel, MoveModel, PlayerSummaryModel, RerollModel, StartEncounterModel } from './apiModels';
import { AUTHORIZATION, ENCOUNTER_ID } from '../Game/utils/constants';
import Cookies from 'js-cookie';

axios.defaults.withCredentials = true;

const api = axios.create({
    baseURL: 'api/',
    headers: {
        'Content-Type': 'application/json',
    },
});

api.interceptors.response.use(response => {
    return response;
 }, error => {
   if (error.response.status === 401) {
    console.log("GOT HERE");
    Cookies.remove(AUTHORIZATION);
    window.location.href = '/';
   }
   return error;
 });

export const getPlayers = async () => {
    return (await api.get<PlayerSummaryModel[]>(`players`)).data;
}

export const getFreeCompanies = async () => {
    return (await api.get<FreeCompanyModel[]>(`free-companies`)).data;
}

export const getEncounters = async () => {
    return (await api.get<EncounterModel[]>(`encounters`)).data;
}

export const createPlayer = async (model: CreatePlayerModel) => {
    return (await api.post(`create-player`, model)).data;
}

export const createFreeCompany = async (model: CreateFreeCompanyModel) => {
    return (await api.post(`create-free-company`, model)).data;
}

export const joinFreeCompany = async (model: JoinFreeCompanyModel) => {
    return (await api.post(`join-free-company`, model)).data;
}

export const startEncounter = async (model: StartEncounterModel) => {
    return (await api.post(`start-encounter`, model)).data;
}

export const startAttack = async (model: AttackModel) => {
    return (await api.post(`start-attack?encounterId=${ENCOUNTER_ID}`, model)).data;
}

export const rerollAttack = async (model: RerollModel) => {
    return (await api.post(`reroll-attack?encounterId=${ENCOUNTER_ID}`, model)).data;
}

export const completeAttack = async (attackId: number) => {
    return (await api.post(`complete-attack?encounterId=${ENCOUNTER_ID}&attackId=${attackId}`)).data;
}

export const move = async (model: MoveModel) => {
    return (await api.post(`move?encounterId=${ENCOUNTER_ID}`, model)).data;
}

export const endTurn = async () => {
    return (await api.post(`end-turn?encounterId=${ENCOUNTER_ID}`)).data;
}

export const continueEnemyAction = async () => {
    return (await api.post(`continue-enemy-action?encounterId=${ENCOUNTER_ID}`)).data;
}