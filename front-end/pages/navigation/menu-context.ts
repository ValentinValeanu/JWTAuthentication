import { createContext } from "react";
import { MenuContextType } from "@/entities/types";

export const MenuContext = createContext<MenuContextType | undefined>(
  undefined,
);
