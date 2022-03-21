import React from 'react';
import { Grid } from '@material-ui/core';
import { useDispatch } from 'react-redux';
import { 
    TSireSeatchLayout, ContentContainer, SearchContainer, BurgerIcon, 
    CloseArrowIcon, SearchIcon, SmallScreenButtonContainer, MaxSizeContainer, MinSizeContainer, SearchContainerFreezeStyle
} from './styledComponents';
import ScreendoxDrawer from '../drawer';
import { sideDrawerIn } from 'actions/settings';
 


const SideSearchLayout = (props: TSireSeatchLayout): React.ReactElement => {

    const dispatch = useDispatch();
    const [isOpen, setOpen] = React.useState(true);
    const [scrollHeight, setScrollHeight] = React.useState(0);
    return (
        <>
        <Grid container style={{ height: 'calc(100vh - 130px)', overflow: 'auto' }}
            onScroll={(e) => {
                let element = e.currentTarget;
                if(props.isFixed) {
                    setScrollHeight(element?.scrollTop);
                }
            }}
        >
            <MaxSizeContainer>
                <ContentContainer isFull={!isOpen}>
                    { props.content }
                </ContentContainer>
                <SearchContainer 
                    isFull={!isOpen} 
                    style={{ 
                        top: scrollHeight
                    }}
                >
                    <BurgerIcon
                        onClick={() => {
                            setOpen(!isOpen);
                        }}
                    >
                        {
                            isOpen ? (
                                <CloseArrowIcon />
                            ) : (
                                <SearchIcon />
                            )
                        }
                    </BurgerIcon>
                    {
                        isOpen ? (props.bar) : null
                    }
                </SearchContainer>
            </MaxSizeContainer>
            <MinSizeContainer
                onKeyDown={ e => {
                    e.stopPropagation()
                }}
            >
                <ScreendoxDrawer anchor="right">
                    <div style={{ paddingTop: '25px' }}>
                        {props.bar}
                    </div>     
                </ScreendoxDrawer>
                <ContentContainer 
                    isFull={!isOpen}
                >
                    { props.content }
                </ContentContainer>
                <SmallScreenButtonContainer
                    onClick={e => {
                        e.stopPropagation();
                        dispatch(sideDrawerIn())
                    } }
                >
                    <SearchIcon />
                </SmallScreenButtonContainer>
            </MinSizeContainer>
        </Grid>
        </>
    )
}

export default SideSearchLayout;
