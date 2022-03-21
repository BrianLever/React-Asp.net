import { createStyles, makeStyles, Theme } from '@material-ui/core/styles';
import styled from 'styled-components';

export type TSireSeatchLayout = {
    content: React.ReactChildren | any;
    bar: React.ReactChildren | any;
    isFixed?: boolean;
}

export type TResizableContainer = {
    isFull: boolean;
}

export const CloseArrowIcon = styled.i`
    display: flex;
    position: relative;
    background-size: 33px;
    background-repeat: no-repeat, repeat;
    width: 46px;
    height: 45px;
    background-image: url(../assets/arrow-circle.svg);
    &:hover {
        background-image: url(../assets/arrow-circle-hover.svg);
    }
`; 

export const SearchIcon = styled.i`
    display: flex;
    position: relative;
    background-size: 33px;
    background-repeat: no-repeat, repeat;
    width: 46px;
    height: 45px;
    background-image: url(../assets/search.svg);
    &:hover {
        background-image: url(../assets/search-hover.svg);
    }
`; 

export const ContentContainer = styled.div`
    flex-grow: 0;
    flex-shrink: 1;
    flex-basis: auto;
    display: block;
    position: relative;
    width: 100%;
    min-width: 0;
    max-width: 100%;
    height: auto;
    min-height: 0;
    max-height: none;
    margin: 0;
    padding: 0;
    z-index: 1;
    border-radius: 10px 0px 0px 10px;
    font-size: 14px;;
    background-color: rgb(255,255,255);
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    margin: calc(1rem / 2) calc(0rem / 2);
    transition: width 2s;
    @media (min-width: 1200px) {
        ${({ isFull }: TResizableContainer) => `flex-basis: ${ isFull ? `calc(97% - 0rem)` : `calc(80% - 0rem)` }`};
    }
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    transition: 0.6s;
`;

export const MaxSizeContainer = styled.div`
    display: none;
    @media (min-width: 1200px) {
        display: flex;
        flex-basis: 100%
    }
`;

export const MinSizeContainer = styled.div`
    display: none;
    @media (max-width: 1200px) {
        display: flex;
        flex-basis: 100%
    }
`;

export const SmallScreenButtonContainer = styled.div`
    display: none;
    @media (max-width: 1200px) {
        display: block;
        position: absolute;
        position: absolute;
        top: 140px;
        right: 48px;
        z-index: 1;
    }
`; 

export const SearchContainer = styled.div`
    flex-grow: 0;
    flex-shrink: 1;
    flex-basis: auto;
    display: block;
    position: relative;
    width: auto;
    min-width: 0;
    max-width: 100%;
    height: auto;
    min-height: 0;
    max-height: none;
    margin: 0;
    padding: 0;
    z-index: 1;
    border-radius: 0px 10px 10px 0px;
    font-size: 16px;;
    background-color: rgb(237,237,242);
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);    
    margin: calc(1rem / 2) calc(0rem / 2);
    position: relative;
    box-sizing: border-box;
    min-height: 1px;
    padding-top: 45px;
    transition: width 2s;
    @media (min-width: 1200px) {
        ${({ isFull }: TResizableContainer) => `flex-basis: ${ isFull ?  `calc(2% - 0rem)` : `calc(19% - 0rem)` }`};
    }
    @media (max-width: 1200px) {
        display: none;
    }
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    transition: 0.6s;
`;


export const SearchContainerFreezeStyle = {
    position: 'absolute', 
    right: 32, 
    width: '16.5%'
}


export const BurgerIcon = styled.div`
    position: absolute;
    border-radius: 50%;
    top: 28px;
    left: -22px;
    width: 46px;
    height: 45px;
    padding: 5px;
    color: #fff;
    background-color: #2E2E42;
    &:hover {
        background-color: #EDEDF2;
    }
    cursor: pointer;
    padding: 7px;
`;

export const TitleText = styled.h1`
  font-family: 'hero-new', sans-serif;
  font-size: 1.2em;
  font-style: normal;
  font-weight: 700;
  line-height: 1.4;
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-transform: capitalize;
  color: #2e2e42;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
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
    },
    notchedOutline: {
        borderWidth: '1px',
        borderColor: 'rgb(46,46,66) !important',
        color: 'rgb(46,46,66) !important'
    },
    cssLabel: {
        color: 'rgb(46,46,66) !important',        
    },
    cssFocused: {
        borderColor: 'rgb(46,46,66) !important',
        color: 'rgb(46,46,66) !important',
        fontSize: '1.1em',
    },
    cssOutlinedInput: {
        fontSize: '1.1em',
        // fontWeight: '800 !important',
    },
    select: {
        "&:before": {
          borderColor: "rgb(46,46,66) !important"
        },
        "&:after": {
            borderColor: "rgb(46,46,66) !important"
        },
        '&:hover:not(.Mui-disabled):before': {
            borderColor: 'rgb(46,46,66) !important',
        }
    }
  }),
);

export const ButtonText = styled.p`
    font-family: "hero-new",sans-serif;
    font-size: 14px;
    font-style: normal;
    font-weight: 700;
    padding: 5px;
    text-transform: none;
    line-height: 1;
    color: rgb(255,255,255);
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const DateToTextStyle = styled.p`
    color: rgb(46,46,66);
    margin: 0;
    padding: 7px 0;
    align-self: center;
    font-size: 1em;
`

export const TemplateDateInputWrapper = styled.div`
  font-family: "hero-new",sans-serif;
  font-size: 1em;
  font-style: normal;
  font-weight: 400;
  line-height: 1.4;
  letter-spacing: 0em;
  text-align: center;
  text-transform: none;
  color: rgba(0,0,0,1);
  background-color: transparent;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  width: 100%;
`

export const TemplateDateInput = styled.input`
  padding: 0 15px;
  text-align: center;
  width: 100%;
  height: 40px;
  font-size: 0.9em !important;
  background: transparent;
  &::-webkit-calendar-picker-indicator {
      display: none;
      -webkit-appearance: none;
  }
  &::-webkit-inner-spin-button {
    display: none;
    -webkit-appearance: none;
  }
  text-align: center;
  transition: border linear 0.2s, box-shadow linear 0.2s;
  display: inline-block;
  border-radius: 4px;
  box-shadow: inset 0 1px 1px rgb(0 0 0 / 8%);
  border: 1px solid rgb(46,46,66);
  font-weight: 800;
  max-height: 45px !important;
  margin: 0 !important;
  z-index: 999;
  color: rgb(46,46,66) !important;
}
}
`

export const TemplateTextInput = styled.input`
    font-size: 14px !important;
    font-weight: 800;
    border: 1px solid rgb(46,46,66) !important;
    font-family: 'Hero New',sans-serif;
    -webkit-appearance: none;
    border-radius: 5px;
    padding-top: 17px !important;
    cursor: text;
    background: transparent !important;
    height: 50px;
    color: rgb(46,46,66) !important;
    width: 100%;
    outline: none;
    box-shadow: inset 0 1px 1px rgb(0 0 0 / 8%), 0 0 8px rgb(0 0 0 / 20%);
    padding-top: 0 !important;
    transition: all 0.2s;
    touch-action: manipulation;
    display: inline-block;
    line-height: normal;
    padding: 0 .65em;
    max-height: 45px !important;
    vertical-align: middle;
`