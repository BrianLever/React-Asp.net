import styled from 'styled-components';
import {reportHeaderTextStyle, baseTextStyle } from '../styledComponents';

export const TopSection = styled.div`
  ${reportHeaderTextStyle};
  background-color: #ededf2;
  border-radius: 5px;
  padding: 16px; 16px; 16px; 16px;;
  background-color: #ededf2;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-bottom: 10px;
`;

export const AnswerLabelSection = styled.div`
  background-color: #ededf2;
  border-radius: 5px 5px 0px 0px;
  padding: 16px; 16px; 16px; 16px;;
  ${baseTextStyle};
  background-color: #ededf2;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-top: 25px;
`;
export const MiddleSection = styled.div`
  border-radius: 5px;
  padding: 16px; 16px; 16px; 16px;;
  font-size: 16px;;
  background-color: #f5f6f8;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-bottom: 10px;
`;

export const TopSectionLabel = styled.h1`
  ${baseTextStyle};
  font-style: normal;
  font-weight: 700;
  line-height: 1.4;
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-transform: none;
  color: #2e2e42;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;



export const ScoreLabelSection = styled.div`
  display: flex;
  flex-direction: row;
  border-radius: 0px 0px 5px 5px;
  font-size: 16px;;
  background-color: #f5f6f8;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  max-height: 68px;
  border: 1px solid #e7e9f0;
  margin-bottom: 10px;
`;

export const ScoreLevel = styled.div`
  ${baseTextStyle}
  display: block;  
  font-weight: 700;
  line-height: 1.4;
  letter-spacing: 0px;
  margin-right: calc(0px * -1);
  text-transform: none;
  color: #2e2e42;
  flex-wrap: wrap;
  text-align: center;
  align-content: center;
  padding: 16px; 16px; 16px; 16px;
  background-color: #f5f6f8;
  width: 273px;
  border-right: 1px solid #e7e9f0;
`;

export const ScoreLevelInlineStypeRules = {
  fontSize: '14px',
  fontStyle: 'normal',
  fontWeight: 700,
  lineHeight: 1.4,
}

export const ScoreIndicates = styled.div`
  ${baseTextStyle} 
  display: block;
  flex-direction: column;
  font-weight: 400;
  line-height: 1.4;
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-transform: none;
  color: #2e2e42;
  flex-wrap: wrap;
  text-align: left;
  align-content: left;
  padding: 16px; 16px; 16px; 16px;
  background-color: #f5f6f8;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  width: 100%;
`;

export const CopyrightSection = styled.div`
  font-size: 12px;
  font-style: normal;
  font-weight: 400;
  line-height: 1.4;
  margin-top: 14px;
`;


export const TopSectionText = styled.div`
  font-size: 16px;
  font-style: normal;
  font-weight: 400;
  line-height: 1.4;
  letter-spacing: 0em;
  text-transform: none;
  color: #2e2e42;
  background-color: transparent;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const ButtonText = styled.span`
  font-size: 16px;;
  font-style: normal;
  font-weight: 700;
  line-height: 1;
  background-color: #2e2e42;
  color: #fff;
`;

export const EhrExportPatientRecordContainer = styled.div`
  padding: 25px;
  width: 100%;
  cursor: pointer;
  border-width: 1px;
  border-style: solid;
  border-color: rgb(46,46,66);
  border-radius: 5px 5px 5px 5px;
  &:hover {
    background-color: rgb(237,237,242);
  }
`

export const EhrExportFinalResultText = styled.h1`
  font-family: "hero-new",sans-serif;
  font-size: 2em;
  font-style: normal;
  font-weight: 700;
  line-height: 1.4;
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-align: center;
  text-transform: none;
  color: rgb(46,46,66);
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const EhrExportFinalResultList = styled.ul`
  list-style: auto;
`