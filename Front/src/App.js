import React from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import LoginComponent from "./Components/Account/Login";
import Layout from "./Components/Layout";
import Home from "./Views/Home";
import { StateProvider } from "./StateContext";

import "./App.css";

function App() {
  return (
    <StateProvider>
      <Layout>
        <BrowserRouter>
          <Routes>
            <Route path="/login" element={<LoginComponent />} />
            <Route path="/" element={<Home />} />
          </Routes>
        </BrowserRouter>
      </Layout>
    </StateProvider>
  );
}

export default App;
