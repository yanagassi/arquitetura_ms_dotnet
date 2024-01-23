import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import "@testing-library/jest-dom";
import SearchDate from "../components/SearchDate";

describe("Componente SearchDate", () => {
  const mockContas = [
    { id: 1, nome: "Conta 1" },
    { id: 2, nome: "Conta 2" },
  ];

  it("renderiza o componente SearchDate", () => {
    render(
      <SearchDate
        contas={mockContas}
        setContaSelect={() => {}}
        setStartDate={() => {}}
        setEndDate={() => {}}
        startDate=""
        endDate=""
        handleSearch={() => {}}
      />
    );

    // Garante que o componente é renderizado corretamente
    expect(screen.getByText("Contas")).toBeInTheDocument();
    expect(screen.getByText("Inicio")).toBeInTheDocument();
    expect(screen.getByText("Fim")).toBeInTheDocument();
    expect(screen.getByText("Buscar")).toBeInTheDocument();
  });

  it("dispara handleSearch quando o botão é clicado", () => {
    const handleSearchMock = jest.fn();
    render(
      <SearchDate
        contas={mockContas}
        setContaSelect={() => {}}
        setStartDate={() => {}}
        setEndDate={() => {}}
        startDate=""
        endDate=""
        handleSearch={handleSearchMock}
      />
    );

    fireEvent.click(screen.getByText("Buscar"));

    expect(handleSearchMock).toHaveBeenCalledTimes(1);
  });
});
