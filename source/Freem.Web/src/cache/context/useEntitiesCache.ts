import {useContext} from "react";
import {EntitiesCacheContext} from "./EntitiesCacheContext.ts";

export function useEntitiesCache() {
  return useContext(EntitiesCacheContext);
}