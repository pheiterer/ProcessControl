export interface Process {
  id: number;
  numeroProcesso: string;
  autor: string;
  reu: string;
  dataAjuizamento: Date;
  status: 'Em andamento' | 'Suspenso' | 'Encerrado';
  descricao: string;
  historico: ProcessHistory[];
}

export interface ProcessHistory {
  id: number;
  processoId: number;
  descricao: string;
  dataInclusao: Date;
  dataAlteracao: Date;
}
