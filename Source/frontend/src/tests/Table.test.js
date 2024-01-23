import React from "react";
import { render, screen } from "@testing-library/react";
import "@testing-library/jest-dom";
import Table from "../components/Table";

const mockData = [
  { id: 1, name: "Item 1", value: 100 },
  { id: 2, name: "Item 2", value: 200 },
  // Adicione mais dados de teste, se necessário
];

const mockColumns = [
  { key: "id", label: "ID", className: "" },
  { key: "name", label: "Name", className: "" },
  { key: "value", label: "Value", className: "" },
];

describe("Componente Table", () => {
  it("renderiza o componente Table com dados", () => {
    render(
      <Table
        data={mockData}
        columns={mockColumns}
        className="custom-table"
        isLoading={false}
      />
    );

    // Garante que o componente é renderizado corretamente
    expect(screen.getByText("ID")).toBeInTheDocument();
    expect(screen.getByText("Name")).toBeInTheDocument();
    expect(screen.getByText("Value")).toBeInTheDocument();

    mockData.forEach((item) => {
      expect(screen.getByText(item.id.toString())).toBeInTheDocument();
      expect(screen.getByText(item.name)).toBeInTheDocument();
      expect(screen.getByText(item.value.toString())).toBeInTheDocument();
    });
  });

  it("renderiza o componente Table sem dados", () => {
    render(
      <Table
        data={[]}
        columns={mockColumns}
        className="custom-table"
        isLoading={false}
      />
    );
  });

  it("renderiza o componente Table no estado de carregamento", () => {
    render(
      <Table
        data={mockData}
        columns={mockColumns}
        className="custom-table"
        isLoading={true}
      />
    );
  });
});
