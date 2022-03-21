import React from 'react';
import { TComponentChildrenType } from '../../../common-types/component.type';
import classes from './Card.module.scss';

export interface ICardProps {
  children?: TComponentChildrenType;
  className?: string;
}

const Card = (props: ICardProps): React.ReactElement => {
  return <div className={`${classes.card} ${props.className}`}>{props.children}</div>;
};

export default Card;
