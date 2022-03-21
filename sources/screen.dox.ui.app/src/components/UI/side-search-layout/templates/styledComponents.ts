import styled from 'styled-components';
import { createStyles, makeStyles, Theme } from '@material-ui/core/styles';

export const TitleText = styled.h1`
    font-family: 'hero-new', sans-serif;
    font-size: 19.6px;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    margin-right: calc(0em * -1);
    text-transform: none;
    padding-bottom: 22px;
    color: #2e2e42;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const TitleTextH2 = styled.h2`
    font-size: 17px;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    margin-right: calc(0em * -1);
    text-transform: none;
    color: #2e2e42;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const ContainerBlock =  styled.div`
  position: relative;
  margin-left: 45px;
  padding-right: 45px;  
`;

export const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    textField: {
      marginLeft: theme.spacing(1),
      marginRight: theme.spacing(1),
      width: '25ch',
    },
    formControl: {
        margin: theme.spacing(1),
        minWidth: '25ch',
    }
  }),
);