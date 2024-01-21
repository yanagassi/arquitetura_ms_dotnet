import React from "react";
import { Link } from "react-router-dom";

const Menu = () => {
  return (
    <div className="bg-primary pl-lg pr-lg h-16 flex items-center justify-between mb-10">
      <div>
        <Link to="/" className="text-white font-bold text-xl">
          <img src="/Images/logo.png" className="w-16" alt="Logo" />
        </Link>
      </div>
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
          to="/leaderboard"
          className="text-white text-lmd hover:text-gray-300  w-100 flex"
        >
          Relatórios
        </Link>
      </div>
    </div>
  );
};

export default Menu;
