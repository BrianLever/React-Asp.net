import styled from 'styled-components';
import {reportTextStyle} from '../../pages/styledComponents';


var nonSelectedAnswerCheckboxLabelColor = '#ededf2';

export const disabledAnswerCheckboxLabel = {
  color: nonSelectedAnswerCheckboxLabelColor
};

export const enabledAnswerCheckboxLabel = {
  color: ''
};


export const AnswerSection = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  ${reportTextStyle}
  border: 1px solid #f5f6f8;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const TopSectionText = styled.div`
  ${reportTextStyle}
  letter-spacing: 0em;
  text-transform: none;
  color: #2e2e42;
  background-color: transparent;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const TopSectionAdditionalText = styled.div`
  ${reportTextStyle}
  letter-spacing: 0em;
  text-transform: none;
  color: #2e2e42;
  background-color: transparent;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-left: 30px;
`;

