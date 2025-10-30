export enum ProcessStatus {
  EmAndamento = 0,
  Suspenso = 1,
  Encerrado = 2,
}

export const ProcessStatusText: Record<number, string> = {
  [ProcessStatus.EmAndamento]: 'Em andamento',
  [ProcessStatus.Suspenso]: 'Suspenso',
  [ProcessStatus.Encerrado]: 'Encerrado',
};

export interface Process {
  id: number;
  numeroProcesso: string;
  autor: string;
  reu: string;
  dataAjuizamento: Date | null;
  status: ProcessStatus | number;
  descricao: string;
}

export class ProcessModel implements Process {
  constructor(
    public id: number,
    public numeroProcesso: string,
    public autor: string,
    public reu: string,
    public dataAjuizamento: Date | null,
    public status: ProcessStatus | number,
    public descricao: string
  ) {}

  get statusText(): string {
    return ProcessStatusText[Number(this.status)] ?? String(this.status);
  }

  static fromDto(dto: any): ProcessModel {
    return new ProcessModel(
      dto.id,
      dto.numeroProcesso,
      dto.autor,
      dto.reu,
      dto.dataAjuizamento ? new Date(dto.dataAjuizamento) : null,
      dto.status,
      dto.descricao
    );
  }
}

export interface ProcessHistory {
  id: number;
  processoId: number;
  descricao: string;
  dataInclusao: Date;
  dataAlteracao: Date;
}
