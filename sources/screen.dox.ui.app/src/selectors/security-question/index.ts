import { IRootState } from '../../states';

export const isChangeSecurityQuestionLoadingSelector  = (state: IRootState) => state.changeSecurityQuestion.isLoading;
export const changeSecurityQuestionListSelector  = (state: IRootState) => state.changeSecurityQuestion.securityQuestionList;