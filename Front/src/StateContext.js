import React, { createContext, useState } from "react";

// Create the state context
export const StateContext = createContext();

// Create a provider component to wrap your app
export const StateProvider = ({ children }) => {
  const [component, setStateData] = useState("");

  return (
    <StateContext.Provider value={{ component, setStateData }}>
      {children}
    </StateContext.Provider>
  );
};
