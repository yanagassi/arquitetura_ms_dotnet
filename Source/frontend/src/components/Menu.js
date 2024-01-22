import React from "react";
import { Link } from "react-router-dom";
import { useApi } from "../context/ApiContext";

const Menu = () => {
  const { isAuthenticated } = useApi();

  return (
    <div className="bg-primary pl-lg pr-lg h-16 flex items-center justify-between mb-10">
      <div>
        <Link to="/" className="text-white font-bold text-xl">
          <img src="/Images/logo.png" className="w-16" alt="Logo" />
        </Link>
      </div>
      {isAuthenticated ? (
        <div className="flex">
          <Link
            to="/"
            className="text-white text-lmd hover:text-gray-300 pr-10 w-100 flex"
          >
            Início
          </Link>
          <Link
            to="/lancamentos"
            className="text-white text-lmd hover:text-gray-300 pr-10 w-100 flex"
          >
            Lançamentos
          </Link>
          <Link
            to="/relatorios"
            className="text-white text-lmd hover:text-gray-300  w-100 flex"
          >
            Relatórios
          </Link>
        </div>
      ) : null}
    </div>
  );
};

export default Menu;
