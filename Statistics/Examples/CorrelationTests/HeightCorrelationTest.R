## load package ggplot2
library("ggplot2")

## load the data
dat = read.csv("height.csv")

weight = dat$Weight
height = dat$Height
shoeSize = dat$ShoeSize

## Boxplot
boxplot(shoeSize,
        col = "orange",
        ylab= "Value",
        xlab= "Shoe Size"
        )

boxplot(height,
        col = "blue",
        ylab= "Value",
        xlab= "Height"
        )


## Scatter plot and line of best fit
plot(height, shoeSize)
abline(lm(shoeSize ~ height))


## Scatter plot and line of best fit
ggplot(data = dat,
       aes(x= height,
           y=shoeSize,
       )
)  +
  # Scatter plot
  geom_point(color = "darkgrey") +
  # Fitted line (linear):
  # The gray area along the line is the confidence interval.
  # If gray area is irrelevant, it may be removed with the 
  # extra argument se=FALSE
  geom_smooth(method="lm",
              col="black",
              #se=FALSE
  )


##  Shapiro-Wilk normality test for the first group
shapiro.test(shoeSize)

##  Shapiro-Wilk normality test for the second group
shapiro.test(height)

##F test to compare the two variances
var.test(shoeSize, height)

## Pearson Correlation
#cor.test(height, shoeSize, method="pearson")

## Spearman Correlation
#cor.test(height, shoeSize, method="spearman")