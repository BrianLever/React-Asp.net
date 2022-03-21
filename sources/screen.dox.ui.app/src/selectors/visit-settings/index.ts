import { IRootState } from '../../states';

export const getVisitSettingsListSelector = (state: IRootState) => state.visitSettings.visitSettingsList;
export const isVisitSettingsListLoadingSelector = (state: IRootState) => state.visitSettings.isLoadingVisitSettingsList;
