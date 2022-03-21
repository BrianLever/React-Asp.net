import { IRootState } from '../../states';


export const getAutoExportLogsSelector = (state: IRootState) => state.autoExportLogs.autoExportLogs;
export const isAutoExportLogsLoadingSelector = (state: IRootState) => state.autoExportLogs.isAutoExportLogsLoading;
export const getAutoExportLogsCurrentPageSelector = (state: IRootState) => state.autoExportLogs.currentPage;
export const getAutoExportLogsStartDateSelector = (state: IRootState) => state.autoExportLogs.startDate;
export const getAutoExportLogsEndDateSelector = (state: IRootState) => state.autoExportLogs.endDate;
export const getAutoExportLogsFilterNameSelector = (state: IRootState) => state.autoExportLogs.filterName;
export const getAutoExportLogsTotalSelector = (state: IRootState) => state.autoExportLogs.total;
export const getAutoExportLogsStatisticsSelector = (state: IRootState) => state.autoExportLogs.statistics;