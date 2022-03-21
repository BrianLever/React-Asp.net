import styled from 'styled-components';

const defaultFonSize = '14px';

export const XMainFull =  styled.div`
    position: relative;
    float: none;
    display: block;
    width: auto;
`;

export const DashboardEntryContainer = styled.div`
&:before {
    content: " ";
    display: table;
    width: 0px;
    box-sizing: border-box;
}
&:after {
    content: " ";
    display: table;
    width: 0px;
    clear: both;
    box-sizing: border-box;
}
font-size: ${defaultFonSize};
box-sizing: border-box;
margin-top: 0;
`;

export const DashboardCSContent =  styled.div`
-webkit-text-size-adjust: 100%;    
font-size: ${defaultFonSize};
font-style: normal;
font-weight: 400;
color: #999999;
`;

export const DashboardSection =  styled.div`
margin: 0px;
padding: 45px 65px 65px 65px;
background-color: #f4f6f8;
z-index: 1;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
transition-property: border-color,background-color,box-shadow,opacity,filter,transform;
transition-duration: 300ms;
display: block;
position: relative;
font-size: ${defaultFonSize};
&:after,
&:before {
    content: " ";
    display: table;
    width: 0px;
    box-sizing: border-box;
}
`;

export const DashboardRowTwo =  styled.div`
z-index: 1;
margin: 0px auto 0px auto;
padding: 1px;
font-size: 16px;;
background-color: transparent;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
transition-property: border-color,background-color,box-shadow,opacity,filter,transform;
transition-duration: 300ms;
display: flex;
position: relative;
flex-flow: row nowrap;
justify-content: center;
align-items: stretch;
width: auto;
min-width: 0;
max-width: none;
height: auto;
min-height: 0;
max-height: none;
border: 0;
border-radius: 0;
box-sizing: border-box;
-webkit-text-size-adjust: 100%;
`;

export const DashboardXRowInner =  styled.div`
flex-direction: row;
justify-content: flex-start;
align-items: stretch;
align-content: stretch;
margin: calc(((1rem / 2) + 1px) * -1) calc(((20px / 2) + 1px) * -1);
display: flex;
flex-wrap: wrap;
flex-grow: 1;
flex-shrink: 1;
flex-basis: auto;
min-width: 0;
min-height: 0;
box-sizing: border-box;
font-style: normal;
font-weight: 400;
color: #999999;
justify-content: center;
`;

export const DashboardXRowInner2 =  styled.div`
font-size: 16px;;
display: flex;
flex-wrap: wrap;
flex-grow: 1;
flex-shrink: 1;
flex-basis: auto;
min-width: 0;
min-height: 0;
flex-direction: row;
justify-content: space-between;
align-items: center;
align-content: center;
margin: calc(((0rem / 2) + 1px) * -1) calc(((0rem / 2) + 1px) * -1);
`;

export const DashboardRowCole23XCol =  styled.div`
    flex-grow: 0;
    flex-shrink: 1;
    position: relative;
    width: auto;
    min-width: 0;
    max-width: 100%;
    height: auto;
    min-height: 0;
    border: 0;
    transition-property: border-color,background-color,box-shadow,opacity,filter,transform;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: flex-start;
    flex-wrap: wrap;
    align-content: flex-start;
    z-index: 1;
    border-radius: 5px 5px 5px 5px;
    padding: 0em 3em 0em 3em;
    font-size: 16px;;
    background-color: rgb(255,255,255);
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    margin: calc(1rem / 2) calc(20px / 2);
    @media (min-width: 1200px) {
        flex-basis: calc(50% - 20px);
    }
    @media (max-width: 1024px) {
        flex-basis: 100vw;
        height: 300px;
    }
`;

export const DashboardRowCole29XCol = styled.div`
    flex-grow: 0;
    flex-shrink: 1;
    display: block;
    position: relative;
    width: auto;
    min-width: 0;
    max-width: 100%;
    height: auto;
    min-height: 0;
    border: 0;
    padding: 0;
    z-index: 1;
    border-radius: 5px 5px 5px 5px;
    padding: 3.4em 3em 0em 0em;
    font-size: 16px;;
    background-color: rgb(255,255,255);
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    margin: calc(1rem / 2) calc(20px / 2);
    max-height: 264px;
    width: 100%;
    @media (min-width: 1200px) {
        flex-basis: calc(50% - 20px);
        height: 100%;
    }
    @media (max-width: 1024px) {
        flex-basis: 100vw;
        height: 300px;
    }
`;

export const DashboardRowCole24XCol =  styled.div`
z-index: 1;
margin: 0px auto 0px auto;
padding: 1px;
font-size: 16px;;
background-color: transparent;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
transition-property: border-color,background-color,box-shadow,opacity,filter,transform;
display: flex;
position: relative;
flex-flow: row nowrap;
justify-content: center;
align-items: stretch;
width: auto;
min-width: 0;
max-width: none;
height: auto;
min-height: 0;
max-height: none;
border: 0;
border-radius: 0;
`;

export const DashboardRowCole25XCol =  styled.div`
display: block;
position: relative;
width: auto;
min-width: 0;
max-width: 100%;
height: auto;
min-height: 0;
max-height: none;
border: 0;
border-radius: 0;
padding: 0;
flex-grow: 0;
flex-shrink: 1;
transition-property: border-color,background-color,box-shadow,opacity,filter,transform;
z-index: 1;
font-size: 16px;;
background-color: transparent;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
margin: calc(0rem / 2) calc(0rem / 2);
flex-basis: calc(70% - 0rem);
`;

export const DashboardRowCole27XCol =  styled.div`
display: block;
flex-grow: 0;
flex-shrink: 1;
width: auto;
min-width: 0;
max-width: 100%;
height: auto;
min-height: 0;
border: 0;
border-radius: 0;
padding: 0;
max-height: none;
transition-property: border-color,background-color,box-shadow,opacity,filter,transform;
position: relative;
bottom: 0;
right: 5em;
font-size: 16px;;
background-color: transparent;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
margin: calc(0rem / 2) calc(0rem / 2);
flex-basis: calc(30% - 0rem);
`;

export const DashboardRowCole26XColXTextHandler =  styled.div`
font-size: 16px;;
background-color: transparent;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
transition-duration: 300ms;
position: relative;
min-width: 1px;
box-sizing: border-box;
`;

export const DashboardXTextContent =  styled.div`
display: flex;
`;

export const DashboardXTextContentText =  styled.div`
display: block;
flex-grow: 1;
min-width: 1px;
max-width: 100%;
`;

export const XTextContentTextPrimary = styled.div`
margin-top: 0;
margin-bottom: 0;
transition-duration: 300ms;
transition-property: color,text-shadow;
font-size: 2em;
font-style: normal;
font-weight: 700;
line-height: 1.4;
letter-spacing: 0em;
margin-right: calc(0em * -1);
text-transform: none;
color: #2e2e42;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const E28XImage = styled.span`
    width: 250px;
    heigh: 216px;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    display: inline-block;
    line-height: 1;
    vertical-align: middle;
`;

export const E28XImageImg = styled.span`
display: block;
max-width: 100%;
height: auto;
vertical-align: bottom;
border: 0;
width: 100%;
object-fit: contain;
& > img {
    width: 100%;
    object-fit: contain;
    display: block;
    max-width: 100%;
    height: auto;
    vertical-align: bottom;
    border: 0;
}
`;

export const DashboardE210XRow = styled.div`
display: flex;
position: relative;
flex-flow: row nowrap;
justify-content: center;
align-items: stretch;
width: auto;
min-width: 0;
max-width: none;
height: auto;
min-height: 0;
max-height: 264px;
margin: 0;
border: 0;
border-radius: 0;
transition-property: border-color,background-color,box-shadow,opacity,filter,transform;
z-index: auto;
margin-left: auto;
margin-right: auto;
padding: 1px;
font-size: 16px;;
background-color: transparent;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);

@media (min-width: 1200px) {
    flex-basis: calc(50% - 2rem);
    height: 100%;
}
`;

export const DashboardE211XCol = styled.div`
flex-grow: 0;
flex-shrink: 1;
position: relative;
width: auto;
min-width: 0;
max-width: 100%;
height: auto;
min-height: 0;
border: 0;
border-radius: 0;
padding: 0;
max-height: none;
display: flex;
flex-direction: column;
justify-content: flex-start;
align-items: center;
flex-wrap: wrap;
align-content: center;
z-index: auto;
font-size: 16px;;
background-color: transparent;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
margin: calc(0rem / 2) calc(2rem / 2);
`;

export const Dashboarde212XTextXTextHeadline = styled.div`
box-sizing: border-box;
min-width: 1px;
position: relative;
transition-property: border-color,background-color,box-shadow,opacity,filter,transform;
padding: 0em 0em 16px; 0em;
font-size: 16px;;
background-color: transparent;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const Dashboardee213XImage = styled.div`
width: 120px;
background-color: transparent;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const DashboardeXTextContentTextPrimary = styled.div`
margin-top: 0;
margin-bottom: 0;
transition-duration: 300ms;
transition-property: color,text-shadow;
font-size: 1.4em;
font-style: normal;
font-weight: 700;
line-height: 1.4;
letter-spacing: 0em;
margin-right: calc(0em * -1);
text-transform: none;
color: #2e2e42;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const DashboardE214XCol = styled.div`
transition-property: border-color,background-color,box-shadow,opacity,filter,transform;
z-index: auto;
font-size: 16px;;
background-color: transparent;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
margin: calc(0rem / 2) calc(2rem / 2);
@media (min-width: 1200px) {
    flex-basis: calc(50% - 2rem);
}
`;

export const DashboardE215XTextXTextHeadline = styled.div`
font-size: 16px;;
background-color: transparent;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const DashboardXTextContentTextPrimary = styled.div`
font-size: 1.4em;
font-style: normal;
font-weight: 700;
line-height: 1.4;
letter-spacing: 0em;
margin-right: calc(0em * -1);
text-transform: none;
color: #2e2e42;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const  DashboardText = styled.div`
font-size: 16px;;
font-style: normal;
font-weight: 400;
line-height: 1.4;
letter-spacing: 0em;
text-transform: none;
color: #2e2e42;
background-color: transparent;
transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;
